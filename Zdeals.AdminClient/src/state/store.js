// configure Redux store

import { createStore, combineReducers, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import { createLogger } from 'redux-logger';

import api from './ducks/api/reducer';
import deals from './ducks/deals/reducer';
import stores from './ducks/stores/reducer';

const reducers = combineReducers({
  api,
  deals,
  stores,
});

const logger = createLogger();

let middleware = [thunk, logger];

export default createStore(
  reducers,
  applyMiddleware(...middleware)
);



