import React from 'react';

import { Row, Col } from 'react-bootstrap';

import Page from '../shared/Page';
import Card from '../shared/Card';
import DealFrom from './DealForm';

import * as dealActions from '../../state/ducks/deals/actions';
import { connect } from 'react-redux';

const buttons = [
  { title: 'Back to List', link: '/deals' }
]

class DealEdit extends React.Component {

  state = {
    deal: null
  };

  componentDidMount() {

    const { match } = this.props;
    const params = match.params;
    const id = params.id;

    if(id) {
      this.props.getDealById(id).then(response => {
        this.setState({ deal: response })
      })
    }
  }

  submitForm = (values) => {
    const deal = this.state.deal;
    if(!deal || !deal.id) return;

    this.props.updateDeal(deal.id, values).then(response => {
      //console.log(response)
    })
  }

  cancelForm = () => {

  }

  render() {
    return (
      <Page title="Edit Deal" buttons={buttons}>
      <Row>
        <Col sm={12} className="grid-margin stretch-card">
          <Card title="Deal Details" className="deal-details">
            <DealFrom initValues={this.state.deal} onSubmit={this.submitForm} onCancel={this.cancelForm} mode="edit"/>  
          </Card>
        </Col>
      </Row>
    </Page>
    )
  }
}

const mapStateToProps = state => ({
  api: state.api
})

const mapDispatchToProps = {
  ...dealActions
}

export default connect(mapStateToProps, mapDispatchToProps)(DealEdit)