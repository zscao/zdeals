import React from 'react';
import { Button } from 'react-bootstrap';

export default function Page(props) {

  function onButtonClick(button) {
    if(typeof(props.onButtonClick) === 'function') props.onButtonClick(button);
  }

  return (
    <div>
      <div className="page-header">
        <h3 className="page-title"> {props.title} </h3>
        <div className="page-buttons">
          {Array.isArray(props.buttons) && props.buttons.map(b => (
            <Button key={b.name} variant='light' onClick={() => onButtonClick(b)}>{b.title}</Button>
          ))}
        </div>
      </div>
      {props.children}
    </div>
  )
}