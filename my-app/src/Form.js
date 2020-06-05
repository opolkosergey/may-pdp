import React from 'react'

const Form = (props) => {
    const { submitText, submitForm } = props
   
    return (
      <form>
          {props.children}
          <input type="button" value={submitText} onClick={submitForm} />
      </form>
    )
  }
  
  export default Form;