import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [assets, setAssets] = useState([])

  useEffect(() => {
    // In local development, the backend will be available at 8080 via docker-compose
    fetch('http://localhost:8080/api/getassetsbyplayer')
      .then(res => res.json())
      .then(data => setAssets(data))
      .catch(err => console.error("Error fetching data:", err));
  }, [])

  return (
    <div className="App">
      <h1>Player Assets Report</h1>
      <table>
        <thead>
          <tr>
            <th>No</th>
            <th>Player name</th>
            <th>Level</th>
            <th>Age</th>
            <th>Asset name</th>
          </tr>
        </thead>
        <tbody>
          {assets.map((item, index) => (
            <tr key={index}>
              <td>{index + 1}</td>
              <td>{item.playerName}</td>
              <td>{item.level}</td>
              <td>{item.age}</td>
              <td>{item.assetName}</td>
            </tr>
          ))}
          {assets.length === 0 && (
            <tr>
              <td colSpan="5">No data available</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  )
}

export default App
