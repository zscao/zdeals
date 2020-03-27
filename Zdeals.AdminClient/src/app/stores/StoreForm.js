import React, { useEffect } from 'react'
import { Form, Button } from 'react-bootstrap';
import { useForm } from 'react-hook-form';

import _ from 'lodash';

import FormErrorBlock from '../shared/FormErrorBlock';

import { storeFormValidation as validation } from './validation';

function StoreForm(props) {

  const { register, handleSubmit, reset, errors } = useForm();

  useEffect(() => {
    const initValues = _.cloneDeep(props.initValues);
    if(initValues) {
      reset(initValues);
    }
  }, [props.initValues])

  function onFormSubmit(values) {
    if(typeof(props.onSubmit) !== 'function') return;
    props.onSubmit(values);
  }

  function onCancel() {
    if(typeof(props.onCancel) === 'function') props.onCancel();
  }

  console.log('errors: ', errors);

  return (
    <Form onSubmit={handleSubmit(onFormSubmit)}>
      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Name</Form.Label>
        <div className="col-sm-10">
          <Form.Control type="text" name="name" isInvalid={!!errors.name} placeholder="Store Name" ref={register(validation.name)} />
          <FormErrorBlock error={errors.name} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Website</Form.Label>
        <div className="col-sm-10">
          <Form.Control type="text" name="website" isInvalid={!!errors.website} placeholder="Website Address" ref={register(validation.website)} />
          <FormErrorBlock error={errors.website} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Domain</Form.Label>
        <div className="col-sm-10">
          <Form.Control type="text" name="domain" isInvalid={!!errors.domain} placeholder="Main Domain Name" ref={register(validation.domain)} />
          <FormErrorBlock error={errors.domain} />
        </div>
      </Form.Group>

      <div className="form-buttons">
        <Button size="lg" variant="light" onClick={onCancel}>Cancel</Button>
        <Button size="lg" type="submit">Submit</Button>
      </div>
    </Form>
  )
}

export default StoreForm;