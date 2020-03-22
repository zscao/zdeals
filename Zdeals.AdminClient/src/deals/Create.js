import React from 'react';
import { Form, InputGroup, Button, Row, Col } from 'react-bootstrap';
import Select from 'react-select';
import { useForm } from 'react-hook-form';

import Page from '../shared/Page';
import Card from '../shared/Card';
import FormErrorBlock from '../shared/FormErrorBlock';

const buttons = [
  { title: 'Back to List', link: '/deals' }
]

const storeOptions = [
  { value: 1, label: 'Taobao' },
  { value: 2, label: 'Amazon' }
]

export default function Create(props) {

  const { register, handleSubmit, errors } = useForm()

  const onFormSubmit = formValues => {
    console.log('form submitting:', formValues);

  }

  return (
    <Page title="Create Deal" buttons={buttons}>
      <Row>
        <Col sm={12} className="grid-margin stretch-card">
          <Card title="Deal Details" className="deal-details">
            <Form onSubmit={handleSubmit(onFormSubmit)}>
              <Form.Group className="row">
                <Form.Label className="col-sm-2 col-form-label">Source</Form.Label>
                <div className="col-sm-10">
                  <Form.Control type="text" name="source" isInvalid={!!errors.source} placeholder="The URL of the original page" ref={register({ required: 'Source is required' })} />
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
                  <Form.Control type="text" placeholder="DD/MM/YYYY" />
                </div>
              </Form.Group>

              <Form.Group className="row">
                <Form.Label className="col-sm-2 col-form-label">Expiry Date</Form.Label>
                <div className="col-sm-3">
                  <Form.Control type="text" placeholder="DD/MM/YYYY" />
                </div>
              </Form.Group>

              <Form.Group className="row">
                <Form.Label className="col-sm-2 col-form-label">Store</Form.Label>
                <div className="col-sm-4">
                  <Select options={storeOptions} />
                </div>
              </Form.Group>

              <div className="form-buttons">
                <Button size="lg" variant="light">Cancel</Button>
                <Button type="submit" size="lg">Submit</Button>
              </div>
            </Form>
          </Card>
        </Col>
      </Row>
    </Page>
  )
}