import React from 'react';

export default function Card(props) {
  return (
    <div className="card">
      <div className="card-body">
        <h4 className="card-title">{props.title}</h4>
        {props.description && <p className="card-description"> {props.description}</p>}
        <div className={props.className}>
          {props.children}
        </div>
      </div>
    </div>
  )
}