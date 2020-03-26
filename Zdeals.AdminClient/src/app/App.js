import React, { Component } from 'react';
import { withRouter } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

import 'react-toastify/dist/ReactToastify.min.css';

import './App.scss';

import AppRoutes from './AppRoutes';
import Navbar from '../shared/Navbar';
import Sidebar from '../shared/Sidebar';
import Footer from '../shared/Footer';
import PageLoader from '../shared/PageLoader';

class App extends Component {
  render() {
    return (
      <div className="container-scroller">
        <Navbar />
        <div className="container-fluid page-body-wrapper">
          <Sidebar />
          <div className="main-panel">
            <div className="content-wrapper">
              <AppRoutes />
            </div>
            <Footer />
          </div>
        </div>
        <PageLoader />
      </div>
    );
  }
}

export default withRouter(App);
