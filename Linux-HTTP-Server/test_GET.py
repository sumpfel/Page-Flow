import requests

def test_is_page_flow_server():
    url = "http://127.0.0.1:5000/is_page_flow_server"  # Update host and port if necessary
    try:
        response = requests.get(url)
        if response.status_code == 200 and response.json() == {"status": "success"}:
            print("Test Passed: Received expected response.")
        else:
            print("Test Failed: Unexpected response.")
            print(f"Status Code: {response.status_code}")
            print(f"Response JSON: {response.json()}")
    except requests.exceptions.RequestException as e:
        print(f"Test Failed: Unable to reach the server. Error: {e}")

if __name__ == "__main__":
    test_is_page_flow_server()