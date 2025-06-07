import requests

def send_feedback(SERVER_URL ,user_name, pwd, book_path, like, comment):
    """Send feedback to the server."""
    payload = {"user_name": user_name,"pwd":pwd, "book_path": book_path, "like": like, "comment": comment}
    response = requests.post(SERVER_URL, json=payload)
    return response.json()


def main():
    user_name = "hater2"
    pwd = "1234"
    book_path = "Cool_Book"
    like = "-1"
    comment = "wer ließt so was !_! x_x"

    print("\nSending feedback to the server...")
    try:
        #response = send_feedback("http://127.0.0.1:5000/feedback", user_name, pwd, book_path, like, comment)
        #print("Response from server:", response)
        response1 = send_feedback("http://127.0.0.1:5000/create_user",user_name, pwd, book_path, like, comment)
        print("Response from server:", response1)
        response = send_feedback("http://127.0.0.1:5000/feedback", user_name, pwd,book_path, like, comment)
        print("Response from server:", response)
    except requests.exceptions.RequestException as e:
        print("Failed to send feedback. Error:", e)


if __name__ == "__main__":
    main()
