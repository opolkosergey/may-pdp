import React, { Component } from 'react'
import './App.css';
import Table from './Table'
import AddClaimForm from './AddClaimForm';
import Form from './Form'
import TabsComponent from './TabsComponent';
import TagsControl from './TagsControl';
import { UserManager } from 'oidc-client';

class App extends Component {
  user = null;

  async componentWillMount() {
    const config = {
      "authority": "https://localhost:5001",
      "client_id": "js",
      "redirect_uri": "http://localhost:3000",
      "scope": "openid profile api1 offline_access",
      "response_type": "code"
    };

    const userManager = new UserManager(config);
    //debugger;
    if (window.location.search.includes("code=")) {
      this.user = await userManager.signinRedirectCallback();
      window.history.replaceState({}, document.title, window.location.pathname);
      await this.getData();
      return;
    } else {
      this.user = await userManager.getUser();
      const isAuth = !!this.user && !!this.user.access_token;

      if (isAuth) {
        await this.getData();
      } else {
        await userManager.signinRedirect();
      }
    }
  }

  async getData(){
    //const accessToken = JSON.parse(sessionStorage.getItem("oidc.user:https://localhost:5001:js")).access_token;
    const accessToken = this.user.access_token;  
    const headers = {Authorization: `Bearer ${accessToken}`}
    
      await fetch("http://localhost:6001/identity", { method: 'GET', headers })
      .then(result => result.json())
      .then(result => {
        this.setState({
          claims: result,
        })
      })
  }

  state = {
    claims:[],
    tags: []
  }

  handleSubmit = claim => {
    this.setState({ claims: [...this.state.claims, claim] })
  }

  removeClaim = index => {
    const { claims } = this.state
  
    this.setState({
      claims: claims.filter((claim, i) => {
        return i !== index
      }),
    })
  }

  handleChangeTags = tags => {
    this.setState({tags})
  }

  submitTagsAndClaims = async () => {
    const headers = {Authorization: `Bearer ${this.user.access_token}`, 'Content-Type': 'application/json'}
    await fetch("http://localhost:6001/identity", { method: 'POST', headers, body: JSON.stringify(this.state) })
      .then(result => result.json())
      .then(result => {
        alert(JSON.stringify(result))
      })
  }

  render() {
    const { claims, tags } = this.state

    return (
      <div className="container">
        {/* <TabsComponent/> */}
        <Form submitText='Submit tags and claims' submitForm={this.submitTagsAndClaims}>
          <Table claims={claims} removeClaim={this.removeClaim} />
          <AddClaimForm handleSubmit={this.handleSubmit} />
          <TagsControl tags={tags} handleChangeTags={this.handleChangeTags} />
        </Form>
      </div>
    )
  }
}

export default App;
