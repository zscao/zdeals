import React from 'react';
import { Link } from 'react-router-dom';

export default function Page(props) {
  return (
    <div>
      <div className="page-header">
        <h3 className="page-title"> {props.title} </h3>
        <div className="page-buttons">
          {Array.isArray(props.buttons) && props.buttons.map(b => (
            <Link key={b.link} className="btn btn-light" to={b.link}>{b.title}</Link>
          ))}
        </div>
      </div>
      {props.children}
    </div>
  )
}