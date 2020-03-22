import React, { Component, Suspense, lazy } from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';

import Spinner from '../shared/Spinner';
const Dashboard = lazy(() => import('../dashboard/Dashboard'));
const DealList = lazy(() => import('../deals/List'));
const DealCreate = lazy(() => import('../deals/Create'));

class AppRoutes extends Component {

  render() {
    return (
      <Suspense fallback={<Spinner/>}>
        <Switch>
          <Route exact path="/dashboard" component={ Dashboard } />
          <Route exact path="/deals" component={DealList} />
          <Route path="/deals/create" component={DealCreate} />
          
          <Redirect to="/dashboard" />
        </Switch>
      </Suspense>
    )
  }
}

export default AppRoutes;