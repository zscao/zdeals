import React from 'react';
import { Row, Col } from 'react-bootstrap';

import Page from '../shared/Page';
import Card from '../shared/Card';
import DealFrom from './DealForm';

import * as dealActions from '../state/ducks/deals/actions';

const buttons = [
  { title: 'Back to List', link: '/deals' }
]

const initValues = {
  publishedDate: new Date(),
}

export default function Create(props) {

  function submitForm(values) {

    dealActions.createDeal(values).then(response => {
      //console.log(response)
    })
  }

  function cancelForm() {

  }

  return (
    <Page title="Create Deal" buttons={buttons}>
      <Row>
        <Col sm={12} className="grid-margin stretch-card">
          <Card title="Deal Details" className="deal-details">
            <DealFrom initValues={initValues} onSubmit={submitForm} onCancel={cancelForm}/>  
          </Card>
        </Col>
      </Row>
    </Page>
  )
}