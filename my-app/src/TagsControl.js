import React from 'react'
import TagsInput from 'react-tagsinput'
 
import 'react-tagsinput/react-tagsinput.css' // If using WebPack and style-loader.
 
const TagsControl = props => {
  const { tags, handleChangeTags } = props
 
  return (
    <TagsInput value={tags} onChange={handleChangeTags} />
  )
}

export default TagsControl;