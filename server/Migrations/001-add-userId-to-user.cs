// // You can run this as a separate console app or a one-time method in your project
// using MongoDB.Bson;
// using MongoDB.Driver;

// var client = new MongoClient("mongodb://localhost:27017"); // Update connection string if needed
// var db = client.GetDatabase("Terminal42DB");
// var users = db.GetCollection<BsonDocument>("Users");
// var counters = db.GetCollection<BsonDocument>("counters");

// // 1. Find the highest existing userId
// var existingUserIds = await users.Find(
//     Builders<BsonDocument>.Filter.Exists("userId") &
//     Builders<BsonDocument>.Filter.Ne("userId", BsonNull.Value)
// ).Project("{userId: 1}")
//  .ToListAsync();

// int maxId = 0;
// foreach (var doc in existingUserIds)
// {
//     var userIdStr = doc.GetValue("userId", "").AsString;
//     if (userIdStr.StartsWith("u") && int.TryParse(userIdStr.Substring(1), out int num))
//     {
//         if (num > maxId) maxId = num;
//     }
// }

// // 2. Find users without a userId
// var filter = Builders<BsonDocument>.Filter.Or(
//     Builders<BsonDocument>.Filter.Exists("userId", false),
//     Builders<BsonDocument>.Filter.Eq("userId", BsonNull.Value)
// );
// var usersWithoutId = await users.Find(filter).ToListAsync();

// // 3. Assign new userIds
// int nextId = maxId + 1;
// foreach (var user in usersWithoutId)
// {
//     string newUserId = $"u{nextId:D5}";
//     var update = Builders<BsonDocument>.Update.Set("userId", newUserId);
//     await users.UpdateOneAsync(
//         Builders<BsonDocument>.Filter.Eq("_id", user["_id"]),
//         update
//     );
//     nextId++;
// }

// // 4. Update the counter
// await counters.UpdateOneAsync(
//     Builders<BsonDocument>.Filter.Eq("_id", "userId"),
//     Builders<BsonDocument>.Update.Set("seq", nextId - 1),
//     new UpdateOptions { IsUpsert = true }
// );

// Console.WriteLine("Migration complete!");