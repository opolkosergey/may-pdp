import React, { Component } from 'react'

class AddClaimForm extends Component {
  initialState = {
    type: '',
    value: '',
  }

  state = this.initialState

  handleChange = event => {
    const { name, value } = event.target
  
    this.setState({
      [name]: value,
    })
  }

  submitForm = () => {
    debugger;
    this.props.handleSubmit(this.state)
    this.setState(this.initialState)
  }

  render() {
    const { type, value } = this.state;
  
    return (
      <div>
        <label htmlFor="type">Type</label>
        <input
          type="text"
          name="type"
          id="type"
          value={type}
          onChange={this.handleChange} />
        <label htmlFor="value">Value</label>
        <input
          type="text"
          name="value"
          id="value"
          value={value}
          onChange={this.handleChange} />
        <input type="button" value="Add claim" onClick={this.submitForm} />
      </div>
    );
  }
}

export default AddClaimForm;