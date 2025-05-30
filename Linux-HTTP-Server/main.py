import os
import csv
from flask import Flask, request, jsonify, send_file, abort
import zipfile
import threading
import time
import sys
import logging

logger = logging.getLogger(__name__)

was_changed = {}
books_dir_path = "books"
users_path = "users.csv"
log_path = "PageFlow.log"
download_dir_path = "downloads"
preview_path = "preview.csv"


def init_files():
    try:
        if not os.path.exists(books_dir_path):
            os.makedirs(books_dir_path)

        if not os.path.exists(os.path.join(download_dir_path, preview_path)):
            if not os.path.exists(download_dir_path):
                os.makedirs(download_dir_path)
            with open(preview_path, mode="w", newline="") as file:
                file.write("")

        if not os.path.exists(users_path):
            with open(users_path, mode="w", newline="") as file:
                file.write("")

        logging.basicConfig(filename=log_path, level=logging.INFO)
        logger.info("Logging started")

        for entry in os.listdir(books_dir_path):
            was_changed[entry] = True


    except Exception as e:
        print("Something went wrong assigning or creating the directories check if program has permissions")
        logger.error(f"file creation failed: {e}")
        exit()

def create_preview_file():
    ratings={}

    #prompt:python for every directory in directory
    for entry in os.listdir(books_dir_path):
        full_path = os.path.join(books_dir_path, entry)
        if os.path.isdir(full_path):

            raiting=0
            settings=[]
            comments=[]
            try:
                with open(os.path.join(full_path,"votes.txt"), mode="r") as file:
                    raiting = int(file.read())
            except:
                pass

            try:
                rows = []
                with open(os.path.join(full_path,"ratings.csv"), mode="r") as file:
                    rows = list(csv.reader(file))
                    for row in rows:
                        if len(row)>=3:
                            for i in range(2,len(row)):
                                comments.append(row[i])
            except:
                pass

            try:
                rows=[]
                with open(os.path.join(full_path,"settings.csv"), mode="r") as file:
                    rows = list(csv.reader(file))
                    settings+=rows[0]

                ratings[os.path.basename(full_path)] = [raiting, settings, comments]
            except:
                logger.error(f"{full_path} has no settings file so it was skipped")

    #prompt: py how can i sort a dictionary by highest value
    sorted_items = sorted(ratings.items(), key=lambda x: x[1][0], reverse=True)

    rows = []
    for key, value in sorted_items:
        rows.append([key, value[0], value[1], value[2]])

    with open(os.path.join(download_dir_path,preview_path), mode="w", newline="") as file:
        csv.writer(file).writerows(rows)


def zip_files_thead(time_):
    def thread_function():
        global was_changed

        while True:
            changed=False

            for key, value in was_changed.items():
                if value:
                    changed=True

                    os.remove(os.path.join(download_dir_path, key+".zip"))
                    zip_files(os.path.join(download_dir_path, key+".zip"), [os.path.join(books_dir_path, key)])

                    was_changed[key] = False

            if changed:
                os.remove(os.path.join(download_dir_path, "all.zip"))
                os.remove(os.path.join(download_dir_path, preview_path))
                create_preview_file()

                all=[]
                for entry in os.listdir(books_dir_path):
                    full_path = os.path.join(books_dir_path, entry)
                    all.append(full_path)
                zip_files(os.path.join(download_dir_path, "all.zip"), all)

            time.sleep(time_)

    thread = threading.Thread(target=thread_function, daemon=True)
    thread.start()
    return thread


def zip_files(zip_path, file_paths):
    with zipfile.ZipFile(zip_path, 'w', zipfile.ZIP_DEFLATED) as zipf:
        for file_path in file_paths:
            # Walk through all subdirs and files inside file_path
            for root, dirs, files in os.walk(file_path):
                for file in files:
                    full_path = os.path.join(root, file)
                    # Add the file to the zip, preserving the relative path inside the folder
                    arcname = os.path.relpath(full_path, os.path.dirname(file_path))
                    zipf.write(full_path, arcname=arcname)
                # Optionally add empty directories:
                if not files and not dirs:
                    # Add empty directory entry
                    dir_arcname = os.path.relpath(root, os.path.dirname(file_path)) + '/'
                    zipf.write(root, arcname=dir_arcname)



def update_ratings(user_name, book_path, like=None, comment=None):
    rows=[]
    user_exists=False
    rating=0
    like_int=0
    if like!=None:
        like_int=int(like)
        if like_int<-2:
            like_int=-2
            like = str(like_int)
        elif like_int>2:
            like_int=2
            like = str(like_int)

    ratings_file=os.path.join(books_dir_path,book_path,"ratings.csv")
    votes_file=os.path.join(books_dir_path, book_path, "votes.txt")

    if os.path.exists(ratings_file) == False:
        with open(ratings_file, "w", newline="") as file:
            file.write("")
    if os.path.exists(votes_file) == False:
        with open(votes_file, "w", newline="") as file:
            file.write("0")

    with open(ratings_file, mode="r") as file:
        rows=list(csv.reader(file))

    for row in rows:
        if row[0]==user_name:
            user_exists=True
            if like != None:
                with open(votes_file, mode="r") as file:
                    rating = int(file.read())
                with open(votes_file, mode="w", newline="") as file:
                    file.write(str(rating - int(row[1]) + like_int))
                row[1]=like
            if comment != None:
                row.append(comment)

    if user_exists == False:
        row = [user_name, "0"]
        if like != None:
            with open(votes_file, mode="r") as file:
                rating = int(file.read())
            with open(votes_file, mode="w", newline="") as file:
                file.write(str(rating + like_int))
            row[1] = like
        if comment != None:
            row.append(comment)
        rows.append(row)

    with open(ratings_file, mode="w", newline="") as file:
        csv.writer(file).writerows(rows)

def check_user(user_name, pwd):
    rows=[]

    with open(users_path, mode="r") as file:
        rows=list(csv.reader(file))
    for row in rows:
        if row[0]==user_name:
            if row[1]==pwd:
                return True
    return False

def create_user(user_name, pwd):
    rows=[]
    with open(users_path, mode="r") as file:
        rows = list(csv.reader(file))

    for row in rows:
        if row[0]==user_name:
            return False

    rows.append([user_name, pwd])
    with open(users_path, mode="w", newline="") as file:
        csv.writer(file).writerows(rows)
    return True

app = Flask(__name__)

@app.route("/download/<library>", methods=['GET'])
def download_books(library):
    file=os.path.join(download_dir_path,f"{library}.zip")
    if os.path.exists(file):
        print(f"{library}.zip was downloaded")
        logging.info(f"file {file} was downloaded")
        return send_file(file, as_attachment=True)
    return abort(404)

@app.route("/preview_books", methods=['GET'])
def preview_books():
    file = os.path.join(download_dir_path, preview_path)
    if os.path.exists(file):
        print(f"file {file} was downloaded")
        return send_file(file, as_attachment=True)
    return abort(404)

@app.route("/feedback", methods=['POST'])
def record_feedback():
    data = request.json
    user_name = data.get("user_name")
    pwd = data.get("pwd")
    book_path = data.get("book_path")
    like = data.get("like")
    comment = data.get("comment")

    if not user_name:
        return jsonify({"error": "MAC address is required"}), 400
    elif not book_path:
        return jsonify({"error": "Book path is required"}), 400

    if check_user(user_name, pwd):
        update_ratings(user_name, book_path, like=like, comment=comment)
        global was_changed
        try:
            was_changed[os.path.basename(book_path)] = True
        except Exception as e:
            logging.error(f"file {book_path} was not updated :{e}")
        return jsonify({"status": "success", "message": "YOUR DATA SOLD WAS TO RUSSIA"})
    else:
        return jsonify({"status": "error", "message": "Invalid username or password"}), 400

@app.route("/create_user",methods=['POST'])
def make_user():
    data = request.json
    user_name = data.get("user_name")
    pwd = data.get("pwd")
    if create_user(user_name, pwd):
        return jsonify({"status": "success"})
    else:
        return jsonify({"status": "user already exists"})

@app.route("/check_user",methods=['GET'])
def if_user():
    data = request.json
    user_name = data.get("user_name")
    pwd = data.get("pwd")
    if check_user(user_name, pwd):
        return jsonify({"status": "success"})
    else:
        return jsonify({"status": "user already exists"})

if __name__ == '__main__':
    init_files()
    zip_files_thead(300)
    app.run(host="0.0.0.0", port=5000)
    logging.info("server started")