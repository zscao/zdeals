import React from 'react';
import { connect } from 'react-redux';

import { Button } from 'react-bootstrap';

import Page from '../shared/Page';
import Card from '../shared/Card';
import { createHistoryJumper } from '../helpers/routeHelper';

import * as storeActions from '../../state/ducks/stores/actions';

const buttons = [
  { name: 'createStore', title: 'Create Store'}
]

class StoreList extends React.Component {

  jumper = createHistoryJumper(this.props.history);

  componentDidMount() {
    this.props.searchStores().then(response => {
      //console.log('stores: ', response);
    });
  }

  onButtonClick = button => {
    if(button.name === 'createStore') this.jumper.jumpTo('/stores/create');
  }

  render() {
    const { stores } = this.props;
    const searchResult = stores.search.result || {};
    const data = searchResult.data || [];

    return (
      <Page title="Store List" buttons={buttons} onButtonClick={this.onButtonClick}>
        <div className="row">
          <div className="col-12 grid-margin stretch-card">
            <Card title="Stores" className="table-responsive">
            <table className="table">
                <thead>
                  <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Website</th>
                    <th>Domain</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {data.map(store => (<tr key={store.id}>
                    <td>{store.id}</td>
                    <td>{store.name}</td>
                    <td>{store.website}</td>
                    <td>{store.domain}</td>
                    <td>
                      <Button variant="primary">Edit</Button>
                    </td>
                  </tr>))}
                </tbody>
              </table>
            </Card>
          </div>
        </div>
      </Page>
    )
  }
}

const mapStateToProps = state => ({
  stores: state.stores
})

const mapDispatchToProps = {
  ...storeActions,
}

export default connect(mapStateToProps, mapDispatchToProps)(StoreList);
