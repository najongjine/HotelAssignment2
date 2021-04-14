window.ShowToastr = (type,message) => {
  if (type == "success") {
    toastr.success(message, 'success second message')
  }
  else if (type == "error") {
    toastr.error(message, 'error second message')
  }
}
