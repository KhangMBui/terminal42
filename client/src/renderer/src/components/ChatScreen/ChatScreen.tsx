/* eslint-disable prettier/prettier */
import React, { useState } from 'react'
import styles from './ChatScreen.module.css'

const ChatScreen: React.FC = () => {
  const [messages, setMessages] = useState<string[]>([])
  const [input, setInput] = useState('')

  const handleSend = (e: React.FormEvent): void => {
    e.preventDefault()
    if (!input.trim()) return
    setMessages([...messages, input])
    setInput('')
  }
  return (
    <div className={styles.terminal}>
      <div className={styles.output}>
        {messages.map((msg, i) => (
          <div key={i} className={styles.message}>
            <span>&gt; </span>
            {msg}
          </div>
        ))}
      </div>
      <form onSubmit={handleSend} className={styles.inputForm}>
        <span>&gt; </span>
        <input
          type="text"
          value={input}
          onChange={(e) => setInput(e.target.value)}
          className={styles.input}
          autoFocus
        />
      </form>
    </div>
  )
}

export default ChatScreen
