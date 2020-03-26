// configure Redux store

import { createStore, combineReducers, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';

import deals from './ducks/deals/reducer';
import api from './ducks/api/reducer';

const reducers = combineReducers({
  api,
  deals,
});

let middleware = [thunk];

export default createStore(
  reducers,
  applyMiddleware(...middleware)
);



