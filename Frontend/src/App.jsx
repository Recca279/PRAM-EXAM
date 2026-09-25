import { useState, useEffect, useMemo } from 'react'
import './App.css'

// Sample data to show when backend has no data or is unreachable
const SAMPLE_DATA = [
  { playerName: "ShadowBlade", level: 45, age: "22", assetName: "Dragon Slayer Sword" },
  { playerName: "ShadowBlade", level: 45, age: "22", assetName: "Phoenix Shield" },
  { playerName: "LunarMage", level: 72, age: "19", assetName: "Staff of Eternity" },
  { playerName: "LunarMage", level: 72, age: "19", assetName: "Moonstone Amulet" },
  { playerName: "IronFist99", level: 33, age: "25", assetName: "Titan Gauntlets" },
  { playerName: "CyberNinja", level: 88, age: "21", assetName: "Plasma Katana" },
  { playerName: "CyberNinja", level: 88, age: "21", assetName: "Stealth Cloak" },
  { playerName: "CyberNinja", level: 88, age: "21", assetName: "Shadow Boots" },
  { playerName: "FrostQueen", level: 56, age: "28", assetName: "Ice Crown" },
  { playerName: "FrostQueen", level: 56, age: "28", assetName: "Blizzard Staff" },
  { playerName: "ThunderGod", level: 95, age: "30", assetName: "Mjolnir Hammer" },
  { playerName: "ThunderGod", level: 95, age: "30", assetName: "Lightning Armor" },
  { playerName: "ThunderGod", level: 95, age: "30", assetName: "Storm Shield" },
  { playerName: "PixelHunter", level: 15, age: "17", assetName: "Wooden Bow" },
  { playerName: "DarkPhoenix", level: 67, age: "24", assetName: "Flame Wings" },
  { playerName: "DarkPhoenix", level: 67, age: "24", assetName: "Rebirth Pendant" },
]

const AVATAR_COLORS = ['avatar-purple', 'avatar-pink', 'avatar-cyan', 'avatar-orange', 'avatar-green', 'avatar-blue']

const ASSET_ICONS = {
  'Sword': '⚔️', 'Shield': '🛡️', 'Staff': '🪄', 'Amulet': '📿',
  'Gauntlets': '🥊', 'Katana': '🗡️', 'Cloak': '🧥', 'Boots': '👢',
  'Crown': '👑', 'Hammer': '🔨', 'Armor': '🛡️', 'Bow': '🏹',
  'Wings': '🪽', 'Pendant': '💎',
}

function getAvatarColor(name) {
  let hash = 0
  for (let i = 0; i < name.length; i++) {
    hash = name.charCodeAt(i) + ((hash << 5) - hash)
  }
  return AVATAR_COLORS[Math.abs(hash) % AVATAR_COLORS.length]
}

function getAssetIcon(assetName) {
  for (const [key, icon] of Object.entries(ASSET_ICONS)) {
    if (assetName.toLowerCase().includes(key.toLowerCase())) return icon
  }
  return '🎮'
}

function getLevelClass(level) {
  if (level >= 80) return 'level-max'
  if (level >= 50) return 'level-high'
  if (level >= 25) return 'level-mid'
  return 'level-low'
}

function App() {
  const [assets, setAssets] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [searchTerm, setSearchTerm] = useState('')
  const [usingSample, setUsingSample] = useState(false)

  useEffect(() => {
    setLoading(true)
    fetch('http://localhost:8080/api/getassetsbyplayer')
      .then(res => {
        if (!res.ok) throw new Error('Server error')
        return res.json()
      })
      .then(data => {
        if (data && data.length > 0) {
          setAssets(data)
          setUsingSample(false)
        } else {
          // No data from backend, use sample data
          setAssets(SAMPLE_DATA)
          setUsingSample(true)
        }
        setLoading(false)
      })
      .catch(err => {
        console.warn("Backend unavailable, using sample data:", err.message)
        setAssets(SAMPLE_DATA)
        setUsingSample(true)
        setLoading(false)
      })
  }, [])

  const filteredAssets = useMemo(() => {
    if (!searchTerm.trim()) return assets
    const term = searchTerm.toLowerCase()
    return assets.filter(item =>
      item.playerName?.toLowerCase().includes(term) ||
      item.assetName?.toLowerCase().includes(term)
    )
  }, [assets, searchTerm])

  // Compute stats
  const stats = useMemo(() => {
    const uniquePlayers = new Set(assets.map(a => a.playerName))
    const totalAssets = assets.length
    const avgLevel = assets.length > 0
      ? Math.round(assets.reduce((sum, a) => sum + (a.level || 0), 0) / assets.length)
      : 0
    const maxLevel = assets.length > 0
      ? Math.max(...assets.map(a => a.level || 0))
      : 0
    return {
      players: uniquePlayers.size,
      assets: totalAssets,
      avgLevel,
      maxLevel,
    }
  }, [assets])

  return (
    <div className="app-container">
      {/* Header */}
      <header className="header">
        <div className="header-badge">
          <span className="dot"></span>
          LIVE REPORT
        </div>
        <h1>Player Assets Report</h1>
        <p>Real-time overview of all player inventories and equipped assets in the BattleGame universe</p>
      </header>

      {/* Stats Grid */}
      <div className="stats-grid">
        <div className="stat-card">
          <div className="stat-icon">👥</div>
          <div className="stat-value">{stats.players}</div>
          <div className="stat-label">Total Players</div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">🎒</div>
          <div className="stat-value">{stats.assets}</div>
          <div className="stat-label">Total Assets</div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">📊</div>
          <div className="stat-value">{stats.avgLevel}</div>
          <div className="stat-label">Avg Level</div>
        </div>
        <div className="stat-card">
          <div className="stat-icon">🏆</div>
          <div className="stat-value">{stats.maxLevel}</div>
          <div className="stat-label">Highest Level</div>
        </div>
      </div>

      {/* Error Banner */}
      {error && (
        <div className="error-banner">
          <span className="error-icon">⚠️</span>
          <span className="error-text">{error}</span>
          <button className="error-dismiss" onClick={() => setError(null)}>✕</button>
        </div>
      )}

      {/* Sample data notice */}
      {usingSample && !loading && (
        <div className="error-banner" style={{ background: 'rgba(245, 158, 11, 0.08)', borderColor: 'rgba(245, 158, 11, 0.2)' }}>
          <span className="error-icon">💡</span>
          <span className="error-text" style={{ color: '#fcd34d' }}>
            Hiển thị dữ liệu mẫu — Backend chưa có data hoặc chưa chạy. Hãy seed data vào database để xem dữ liệu thật.
          </span>
        </div>
      )}

      {/* Table Card */}
      <div className="table-card">
        <div className="table-header">
          <div className="table-header-left">
            <div className="table-icon">📋</div>
            <h2>Inventory List</h2>
          </div>
          <div className="table-header-right">
            <div className="search-box">
              <span className="search-icon">🔍</span>
              <input
                id="search-input"
                type="text"
                placeholder="Search player or asset..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </div>
          </div>
        </div>

        {loading ? (
          <div className="loading-container">
            <div className="loading-spinner"></div>
            <div className="loading-text">Loading player data...</div>
          </div>
        ) : filteredAssets.length === 0 ? (
          <div className="empty-state">
            <div className="empty-icon">🎮</div>
            <h3>No Data Found</h3>
            <p>{searchTerm ? 'No results match your search.' : 'No player assets in the database yet.'}</p>
          </div>
        ) : (
          <>
            <div style={{ overflowX: 'auto' }}>
              <table className="data-table">
                <thead>
                  <tr>
                    <th>No</th>
                    <th>Player Name</th>
                    <th>Level</th>
                    <th>Age</th>
                    <th>Asset Name</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredAssets.map((item, index) => (
                    <tr key={index}>
                      <td>
                        <span className="row-number">{index + 1}</span>
                      </td>
                      <td>
                        <div className="player-name-cell">
                          <div className={`player-avatar ${getAvatarColor(item.playerName || '')}`}>
                            {(item.playerName || '?')[0].toUpperCase()}
                          </div>
                          <span className="player-name">{item.playerName}</span>
                        </div>
                      </td>
                      <td>
                        <span className={`level-badge ${getLevelClass(item.level)}`}>
                          <span className="level-dot"></span>
                          LV. {item.level}
                        </span>
                      </td>
                      <td>
                        <span className="age-text">{item.age}</span>
                      </td>
                      <td>
                        <div className="asset-cell">
                          <span className="asset-icon">{getAssetIcon(item.assetName || '')}</span>
                          <span className="asset-name">{item.assetName}</span>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
            <div className="table-footer">
              <div className="table-footer-info">
                Showing {filteredAssets.length} of {assets.length} records
              </div>
              <div className="table-footer-info">
                {usingSample ? '📦 Sample Data' : '🔗 Live Data'}
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  )
}

export default App
