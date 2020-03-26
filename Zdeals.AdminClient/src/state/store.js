// configure Redux store

import { createStore, combineReducers, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';

import deals from './ducks/deals/reducer';

const reducers = combineReducers({
  deals,
});

let middleware = [thunk];

export default createStore(
  reducers,
  applyMiddleware(...middleware)
);



