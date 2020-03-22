import React from 'react';

import { Form } from 'react-bootstrap';

export default function FormErrorBlock(props) {
  const error = props.error;
  if(!error) return null;

  console.log(error);

  return (
    <Form.Control.Feedback type="invalid">{error.message}</Form.Control.Feedback>
  )
}