/* eslint-disable prettier/prettier */
import React, { useEffect, useRef, useState } from 'react'
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr'
import styles from './ChatScreen.module.css'

interface Message {
  user: string
  text: string
}

const ChatScreen: React.FC = () => {
  const [connection, setConnection] = useState<HubConnection | null>(null)
  const [messages, setMessages] = useState<Message[]>([])
  const [input, setInput] = useState('')
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [username, setUsername] = useState('User' + Math.floor(Math.random() * 1000))
  const outputRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5213/hubs/chat') // Use your backend URL/port
      .withAutomaticReconnect()
      .build()

    setConnection(newConnection)

    return () => {
      newConnection.stop()
    }
  }, [])

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log('SignalR Connected!')
          connection.on('ReceiveMessage', (user: string, text: string) => {
            setMessages((prev) => [...prev, { user, text }])
          })
        })
        .catch((err) => console.error('SignalR Connection Error: ', err))
    }
  }, [connection])

  useEffect(() => {
    // Scroll to bottom on new message
    outputRef.current?.scrollTo(0, outputRef.current.scrollHeight)
  }, [messages])

  const handleSubmit = async (e: React.FormEvent): Promise<void> => {
    e.preventDefault()
    if (input.trim() && connection) {
      try {
        await connection.invoke('SendMessage', username, input)
        setInput('')
      } catch (err) {
        console.error('SendMessage failed:', err)
      }
    }
  }

  return (
    <div className={styles.terminal}>
      <div className={styles.output} ref={outputRef}>
        {messages.map((msg, idx) => (
          <div key={idx} className={styles.message}>
            <strong>{msg.user}:</strong> {msg.text}
          </div>
        ))}
      </div>
      <form className={styles.inputForm} onSubmit={handleSubmit}>
        <input
          className={styles.input}
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="Type a message..."
          autoFocus
        />
        <button type="submit" style={{ display: 'none' }}>
          Send
        </button>
      </form>
    </div>
  )
}

export default ChatScreen
