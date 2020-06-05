import React, { Component } from 'react'

const TableHeader = () => {
    return (
      <thead>
        <tr>
          <th>Type</th>
          <th>Value</th>
        </tr>
      </thead>
    )
  }

  const TableBody = props => {
    const rows = props.claims.map((row, index) => {
      return (
        <tr key={index}>
        <td>{row.type}</td>
        <td>{row.value}</td>
            <td>
                <button onClick={() => props.removeClaim(index)}>Remove from memory</button>
            </td>
        </tr>
      )
    })
  
    return <tbody>{rows}</tbody>
  }

  const Table = props => {
    const { claims, removeClaim } = props
  
    return (
      <table>
        <TableHeader />
        <TableBody claims={claims} removeClaim={removeClaim} />
      </table>
    )
  }

export default Table