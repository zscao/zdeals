import React, { useEffect } from 'react';
import { Form, InputGroup, Button } from 'react-bootstrap';
import { useForm } from 'react-hook-form';

import moment from 'moment';
import _ from 'lodash';

import FormErrorBlock from '../shared/FormErrorBlock';

export default function DealForm(props) {

  const { register, handleSubmit, reset, errors } = useForm()

  useEffect(() => {
    const initValues = _.cloneDeep(props.initValues);
    if(initValues) {
      initValues.publishedDate = moment(initValues.publishedDate).format('YYYY-MM-DD');
      if(initValues.expiryDate) initValues.expiryDate = moment(initValues.expiryDate).format('YYYY-MM-DD');

      reset(initValues)
    }
  }, [props.initValues])

  function onFormSubmit(values) {

    if(typeof(props.onSubmit) !== 'function') return;

    if(values.dealPrice) values.dealPrice = Number.parseFloat(values.dealPrice);
    if(values.fullPrice) values.fullPrice = Number.parseFloat(values.fullPrice);

    if(!values.expiryDate) values.expiryDate = null;

    props.onSubmit(values);    
  }

  function onCancel() {
    if(typeof(props.onCancel) !== 'function') return;

    props.onCancel();
  }

  return (
    <Form onSubmit={handleSubmit(onFormSubmit)}>
      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Source</Form.Label>
        <div className="col-sm-10">
          <Form.Control type="text" name="source" readOnly={props.mode === 'edit'} isInvalid={!!errors.source} placeholder="The URL of the original page" ref={register({ required: 'Source is required' })} />
          <FormErrorBlock error={errors.source} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Title</Form.Label>
        <div className="col-sm-10">
          <Form.Control type="text" name="title" isInvalid={!!errors.title} placeholder="Title" ref={register({ required: 'Title is required' })} />
          <FormErrorBlock error={errors.title} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Highlight</Form.Label>
        <div className="col-sm-10">
          <Form.Control type="text" name="highlight" isInvalid={!!errors.highlight} placeholder="Highlight" ref={register({ required: 'Highlight is required' })} />
          <FormErrorBlock error={errors.highlight} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Deal Price</Form.Label>
        <div className="col-sm-3">
          <InputGroup className="md-3">
            <InputGroup.Prepend>
              <InputGroup.Text>$</InputGroup.Text>
            </InputGroup.Prepend>

            <Form.Control type="text" name="dealPrice" isInvalid={!!errors.dealPrice} placeholder="0.00" ref={register({ pattern: { value: /^\d*(\.\d{1,2})?$/, message: 'Deal price must be numbers' } })} />
            <FormErrorBlock error={errors.dealPrice} />
          </InputGroup>
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Full Price</Form.Label>
        <div className="col-sm-3">
          <InputGroup className="md-3">
            <InputGroup.Prepend>
              <InputGroup.Text>$</InputGroup.Text>
            </InputGroup.Prepend>

            <Form.Control type="text" name="fullPrice" isInvalid={!!errors.fullPrice} placeholder="0.00" ref={register({ pattern: { value: /^\d*(\.\d{1,2})?$/, message: 'Full price must be numbers' } })} />
            <FormErrorBlock error={errors.fullPrice} />
          </InputGroup>
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Discount</Form.Label>
        <div className="col-sm-3">
          <Form.Control type="text" name="discount" placeholder="" ref={register} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Description</Form.Label>
        <div className="col-sm-10">
          <Form.Control as="textarea" name="description" rows="4" placeholder="" ref={register} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Published Date</Form.Label>
        <div className="col-sm-3">
          <Form.Control type="date" name="publishedDate" isInvalid={!!errors.publishedDate} placeholder="DD/MM/YYYY" ref={register({ required: 'Published date is required' })} />
          <FormErrorBlock error={errors.publishedDate} />
        </div>
      </Form.Group>

      <Form.Group className="row">
        <Form.Label className="col-sm-2 col-form-label">Expiry Date</Form.Label>
        <div className="col-sm-3">
          <Form.Control type="date" name="expiryDate" isInvalid={!!errors.expiryDate} placeholder="DD/MM/YYYY" ref={register} />
          <FormErrorBlock error={errors.expiryDate} />
        </div>
      </Form.Group>

      <div className="form-buttons">
        <Button size="lg" variant="light">Cancel</Button>
        <Button type="submit" size="lg" onClick={onCancel}>Submit</Button>
      </div>
    </Form>
  )
}