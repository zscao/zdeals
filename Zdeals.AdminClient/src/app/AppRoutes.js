import React, { Component, Suspense, lazy } from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';

import Spinner from './shared/Spinner'

const Dashboard = lazy(() => import('./dashboard/Dashboard'))

const DealList = lazy(() => import('./deals/List'))
const DealCreate = lazy(() => import('./deals/Create'))
const DealEdit = lazy(() => import('./deals/Edit'))

const StoreList = lazy(() => import('./stores/List'))
const StoreCreate = lazy(() => import('./stores/Create'))

class AppRoutes extends Component {

  render() {
    return (
      <Suspense fallback={<Spinner/>}>
        <Switch>
          <Route exact path="/dashboard" component={ Dashboard } />
          
          <Route exact path="/deals" component={DealList} />
          <Route path="/deals/create" component={DealCreate} />
          <Route path="/deals/edit/:id" component={DealEdit} />

          <Route exact path="/stores" component={StoreList} />
          <Route path="/stores/create" component={StoreCreate} />

          <Redirect to="/dashboard" />
        </Switch>
      </Suspense>
    )
  }
}

export default AppRoutes;