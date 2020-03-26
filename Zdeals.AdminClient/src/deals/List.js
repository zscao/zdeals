import React from 'react';
import { connect } from 'react-redux';

import { Button } from 'react-bootstrap';

import Page from '../shared/Page';
import Card from '../shared/Card';

import * as dealActions from '../state/ducks/deals/actions';

const buttons = [
  { title: 'Add Deal', link: '/deals/create' }
]

class DealList extends React.Component {

  componentDidMount() {
    this.props.searchDeals({pageSize: 20});
  }

  editDeal = deal => {
    this.jumpTo('/deals/edit/' + deal.id)
  }

  jumpTo = next => {
    if(this.props.history) {
      this.props.history.push(next);
    }
  }

  render() {

    const { deals } = this.props;
    const searchResult = deals.search.result || {};
    const data = searchResult.data || [];


    return (
      <Page title="Deal List" buttons={buttons}>
        <div className="row">
          <div className="col-12 grid-margin stretch-card">
            <Card title="Basic Table" className="table-responsive">
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