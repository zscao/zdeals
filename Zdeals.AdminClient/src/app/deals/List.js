import React from 'react';
import { connect } from 'react-redux';

import { Button } from 'react-bootstrap';

import Page from '../shared/Page';
import Card from '../shared/Card';

import * as dealActions from '../../state/ducks/deals/actions';
import { createHistoryJumper } from '../helpers/routeHelper';

const buttons = [
  { name: 'createDeal', title: 'Create Deal' }
]

class DealList extends React.Component {

  jumper = createHistoryJumper(this.props.history);

  componentDidMount() {
    this.props.searchDeals({pageSize: 20});
  }

  editDeal = deal => {
    this.jumper.jumpTo('/deals/edit/' + deal.id)
  }

  onButtonClick = button => {
    if(button.name === 'createDeal')
      this.jumper.jumpTo('/deals/create');
  }

  render() {

    const { deals } = this.props;
    const searchResult = deals.search.result || {};
    const data = searchResult.data || [];

    return (
      <Page title="Deal List" buttons={buttons} onButtonClick={this.onButtonClick}>
        <div className="row">
          <div className="col-12 grid-margin stretch-card">
            <Card title="Deals" className="table-responsive">
              <table className="table">
                <thead>
                  <tr>
                    <th>Id</th>
                    <th>Title</th>
                    <th>Published</th>
                    <th>Store</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {data.map(deal => (<tr key={deal.id}>
                    <td>{deal.id}</td>
                    <td className="text-overflow" style={{width: '50%'}}>{deal.title}</td>
                    <td>{deal.publishedDate}</td>
                    <td>{deal.store && deal.store.name}</td>
                    <td>
                      <Button variant="primary" onClick={() => this.editDeal(deal)}>Edit</Button>
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
  deals: state.deals
});

const mapDispatchToProps = {
  ...dealActions
}

export default connect(mapStateToProps, mapDispatchToProps)(DealList);