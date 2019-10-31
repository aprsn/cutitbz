function copyLink() {
    /* Get the text field */
    var copyText = document.querySelector('.shortLink');
  
    /* Select the text field */
    copyText.select();
    copyText.setSelectionRange(0, 99999); /*For mobile devices*/
  
    /* Copy the text inside the text field */
    document.execCommand("copy");
    alertBox('Link copied successfully!', 'success');
    return false; 
  }



  var alreadySet = false;

  function alertBox(message, type) {
    var alertbox = document.createElement('div');
    var messageSpan = document.createElement('span');
    messageSpan.innerHTML = message;
    alertbox.appendChild(messageSpan);
  
    if (type === "success") {
      alertbox.className = "alertbox success";
    } else {
      alertbox.className = "alertbox error";
    }
  
    var oldAlert = document.querySelector('.alertbox');
  
    if (oldAlert !== null) {
      oldAlert.remove();
    }
  
    var alertboxcount = 4;
  
    if (alreadySet == false) {
      alreadySet = true;
      var alertboxTimer = setInterval(function () {
        alertboxcount--;
  
        if (alertboxcount <= 0) {
          document.querySelector('.alertbox').remove();
          clearInterval(alertboxTimer);
          alreadySet = false;
        }
      }, 1000);
    }
  
    document.body.appendChild(alertbox);
  }

function linkAlert(value, status) {
    var linkAlertSpan = document.querySelector("#linkAlert");
  if(status) {
    linkAlertSpan.classList.remove("invalid");
    linkAlertSpan.classList.add("valid");
    linkAlertSpan.innerHTML = value;
  } else { 
    linkAlertSpan.classList.remove("valid");
    linkAlertSpan.classList.add("invalid");
    linkAlertSpan.innerHTML = value;
  }
}

function checkLink(link) {
  var fullLink = link.split('://');
  var linkForm = document.getElementById("linkForm");

  if(fullLink.length < 2) {
    linkForm.setAttribute("onsubmit", "return false;");
    linkAlert("Link is invalid!", false);
    return false;
  } else {
    if((fullLink[0] != 'http' && fullLink[0] != 'https') || fullLink[1] == "" || fullLink[1].split(".")[1] == null || fullLink[1].split(".")[1] == "" ) {
      linkForm.setAttribute("onsubmit", "return false;");
      linkAlert("Link is invalid!", false);
      return false;
    }
  }

  linkAlert("Link is valid!", true);
  linkForm.removeAttribute("onsubmit");
  return false;
}