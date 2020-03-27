import React from 'react';
import { connect } from 'react-redux';

import spinner from '../../assets/images/spinner.gif';

const PageLoader = props => {

  if(!Array.isArray(props.loadings) || !props.loadings.length) return null;
  
  return (
    <div className="loader-container">
      <div className="loader">
        <img src={spinner} width="120" alt="" />
      </div>
    </div>
  )
}

const mapStateToProps = state => ({
  loadings: state.api.loadings
})

export default connect(mapStateToProps)(PageLoader);