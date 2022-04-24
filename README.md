# Dashboard![Dashboard-readme](https://user-images.githubusercontent.com/90765194/164998076-ae6baa5b-2efb-4608-9443-89aa4d64d98f.png)
How to use it:
- The Kafka Consumer and Producer fetches a .txt file from users/youruser with the WSL ip address.
- The Kafka consumes one message and stops after 1 sec, to retrieve just the latest price reading
- Consumer and Producer are on the same application (Publisher - Subscriber)
- Improvements acchieved: Passwords and user names were stored in secrets.json and retrieved to the static methods from Iconfiguration
- To-do: Convert euros to DKK. The DKK json results were very unestable and would often crash the app, therefore I decided not to use this field.
- SOAP applications work with local host, which has to be changed on code to fit the local host on another computer
