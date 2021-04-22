window.ShowToastr = (type,message) => {
  if (type == "success") {
    toastr.success(message, 'success second message')
  }
  else if (type == "error") {
    toastr.error(message, 'error second message')
  }
}

window.ShowSweetAlert = (type, message) => {
  if (type == "success") {
    Swal.fire({
      title: 'sweet alert success title',
      text: message,
      icon: 'success',
      confirmButtonText: 'Cool'
    })
  }
  else if (type == "error") {
    Swal.fire({
      title: 'sweet alert error title',
      text: message,
      icon: 'error',
      confirmButtonText: 'Cool'
    })
  }
}

function ShowDeleteConfirmationModal() {
  $("#deleteConfirmationModal").modal('show')
}
function HideDeleteConfirmationModal() {
  $("#deleteConfirmationModal").modal('hide')
}
