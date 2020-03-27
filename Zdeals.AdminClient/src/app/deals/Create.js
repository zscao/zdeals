import React from 'react';
import { connect } from 'react-redux';
import { Row, Col } from 'react-bootstrap';

import Page from '../shared/Page';
import Card from '../shared/Card';
import DealFrom from './DealForm';
import { createHistoryJumper } from '../helpers/routeHelper';

import * as dealActions from '../../state/ducks/deals/actions';

const buttons = [
  { name: "backToList", title: 'Back to List' }
]

const initValues = {
  publishedDate: new Date(),
}

class Create extends React.Component {

  jumper = createHistoryJumper(this.props.history);

  submitForm = values => {
    this.props.createDeal(values).then(response => {
      //console.log(response)
    })
  }

  cancelForm = () => {
    this.jumper.jumpTo('/deals');
  }

  onButtonClick = button => {
    if (button.name === 'backToList') this.jumper.jumpTo('/deals');
  }

  render() {
    return (
      <Page title="Create Deal" buttons={buttons} onButtonClick={this.onButtonClick}>
        <Row>
          <Col sm={12} className="grid-margin stretch-card">
            <Card title="Deal Details" className="deal-details">
              <DealFrom initValues={initValues} onSubmit={this.submitForm} onCancel={this.cancelForm} />
            </Card>
          </Col>
        </Row>
      </Page>
    )
  }
}

const mapStateToProps = state => ({
  deals: state.deals
})

const mapDispatchToProps = {
  ...dealActions
}

export default connect(mapStateToProps, mapDispatchToProps)(Create);