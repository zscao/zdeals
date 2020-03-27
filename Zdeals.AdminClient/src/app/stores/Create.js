import React from 'react';
import { connect } from 'react-redux';
import { Row, Col } from 'react-bootstrap';

import { createHistoryJumper } from '../helpers/routeHelper';

import Page from '../shared/Page';
import Card from '../shared/Card';
import StoreForm from './StoreForm';

import * as storeActions from '../../state/ducks/stores/actions';


const buttons = [
  { name: 'backToList', title: 'Back to List'}
]

class Create extends React.Component {

  jumper = createHistoryJumper(this.props.history);

  submitForm = values => {
    //console.log('submitting: ', values);
    this.props.createStore(values).then(response => {
      console.log(response);
    })
  }

  cancelForm = () => {
    this.jumper.jumpTo('/stores')
  }

  onButtonClick = button => {
    if(button.name === 'backToList') this.jumper.jumpTo('/stores')
  }

  render() {
    return (
      <Page title="Create Store" buttons={buttons} onButtonClick={this.onButtonClick}>
        <Row>
          <Col sm={12} className="grid-margin stretch-card">
            <Card title="Store Details" className="store-details">
              <StoreForm onSubmit={this.submitForm} onCancel={this.cancelForm}/>
            </Card>
          </Col>
        </Row>

      </Page>
    )
  }
}

const mapStateToPros = state => ({
  stores: state.stores
})

const mapDispatchToProps = {
  ...storeActions
}
export default connect(mapStateToPros, mapDispatchToProps)(Create);