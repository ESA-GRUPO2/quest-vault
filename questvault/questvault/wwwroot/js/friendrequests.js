
$(document).ready(

  function () {

    fetch('/Friendships/GetFriendRequests')
      .then(response => response.json())
      .then(data => {

        if (data.length > 0) {

          document.getElementById('friendsText').classList.add('blink-text');
          document.getElementById('pendingIndicator').style.backgroundColor = '#98008D';
        } else {

          document.getElementById('friendsText').classList.remove('blink-text');
          document.getElementById('pendingIndicator').style.backgroundColor = 'transparent';
        }
      })
      .catch(error => console.error('Error verifying friend requests:', error));
  }
);

setInterval(
  function () {


    fetch('/Friendships/GetFriendRequests')
      .then(response => response.json())
      .then(data => {

        if (data.length > 0) {


          document.getElementById('friendsText').classList.add('blink-text');
          document.getElementById('pendingIndicator').style.backgroundColor = '#98008D';
        } else {

          document.getElementById('friendsText').classList.remove('blink-text');
          document.getElementById('pendingIndicator').style.backgroundColor = 'transparent';
        }
      })
      .catch(error => console.error('Error verifying friend requests:', error));
  }
  , 60000);