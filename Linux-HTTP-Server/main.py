import os
import csv
from flask import Flask, request, jsonify, send_file, abort
import zipfile
import threading
import time
import sys
import logging

logger = logging.getLogger(__name__)
logger.setLevel(logging.FATAL)

was_changed = {}
books_dir_path = "books"
users_path = "users.csv"
log_path = "PageFlow.log"
download_dir_path = "downloads"
preview_path = "preview.csv"


def init_files():
    try:
        with open(log_path, mode="w", newline="") as file:
            file.write("")

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

            rating=[0,0,0]
            settings=[]
            comments=[]
            try:
                with open(os.path.join(full_path,"votes.txt"), mode="r") as file:
                    rating = list(csv.reader(file))[0]
                rating = [int(rating[0]),rating[1],rating[2]]
            except:
                pass

            try:
                with open(os.path.join(full_path,"comments.csv"), mode="r") as file:
                    comments = list(csv.reader(file,delimiter="*"))

            except Exception as e:
                pass

            try:
                rows=[]
                with open(os.path.join(full_path,"settings.csv"), mode="r") as file:
                    rows = list(csv.reader(file))
                    settings+=rows[0]

                settings_=""
                for i in settings:
                    settings_+=i+";"
                comments_=""
                for comment in comments:
                    comments_+=f"{comment[0]}@@{comment[1]};"
                ratings[os.path.basename(full_path)] = rating+[settings_, comments_]
            except Exception as e:
                logger.error(f"{full_path} has no settings file so it was skipped {e}")

    #prompt: py how can i sort a dictionary by highest value
    sorted_items = sorted(ratings.items(), key=lambda x: x[1][0], reverse=True)
    rows = []
    for key, value in sorted_items:
        rows.append([key, value[0],value[1],value[2], value[3], value[4]])

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
                    try:
                        os.remove(os.path.join(download_dir_path, key+".zip"))
                    except:pass
                    zip_files(os.path.join(download_dir_path, key+".zip"), [os.path.join(books_dir_path, key)])

                    was_changed[key] = False

            if changed:
                try:
                    os.remove(os.path.join(download_dir_path, "all.zip"))
                except:pass
                try:
                    os.remove(os.path.join(download_dir_path, preview_path))
                except:pass
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
    comments_file = os.path.join(books_dir_path, book_path, "comments.csv")

    if os.path.exists(ratings_file) == False:
        with open(ratings_file, "w", newline="") as file:
            file.write("")
    if os.path.exists(votes_file) == False:
        with open(votes_file, "w", newline="") as file:
            file.write("0,0,0")
    if os.path.exists(comments_file) == False:
        with open(comments_file, "w", newline="") as file:
            file.write("")

    with open(ratings_file, mode="r") as file:
        rows=list(csv.reader(file))

    for row in rows:
        if row[0]==user_name:
            user_exists=True
            if like != None:
                with open(votes_file, mode="r") as file:
                    ratings = list(csv.reader(file))[0]
                new_ratings=[[0,ratings[1],ratings[2]]]
                old_vote=int(row[1])
                new_ratings[0][0]=int(ratings[0]) - old_vote + like_int

                if old_vote<0:
                    new_ratings[0][2]=int(ratings[2]) + old_vote
                else:
                    new_ratings[0][1]=int(ratings[1]) - old_vote

                if like_int<0:
                    new_ratings[0][2]=int(new_ratings[0][2])- like_int
                else:
                    new_ratings[0][1]=int(new_ratings[0][1]) + like_int

                with open(votes_file, mode="w", newline="") as file:
                    csv.writer(file).writerows(new_ratings)

                row[1]=like_int
            if comment != None:
                row.append(comment)

                with open(comments_file, mode="r") as file:
                    rows2 = list(csv.reader(file,delimiter="*"))
                rows2.append([user_name,comment])
                with open(comments_file, mode="w", newline="") as file:
                    csv.writer(file,delimiter="*").writerows(rows2)

    if user_exists == False:
        row = [user_name, "0"]
        if like != None:
            with open(votes_file, mode="r") as file:
                ratings = list(csv.reader(file))[0]
            new_ratings = [[0, ratings[1], ratings[2]]]
            new_ratings[0][0] = int(ratings[0]) - int(row[1]) + like_int

            if like_int < 0:
                new_ratings[0][2] = int(ratings[2]) - like_int
            else:
                new_ratings[0][1] = int(ratings[1]) + like_int

            with open(votes_file, mode="w", newline="") as file:
                csv.writer(file).writerows(new_ratings)
            row[1] = like_int
        if comment != None:
            row.append(comment)

            with open(comments_file, mode="r") as file:
                rows2 = list(csv.reader(file))
            rows2.append([user_name, comment])
            with open(comments_file, mode="w", newline="") as file:
                csv.writer(file).writerows(rows2)

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
        return jsonify({"error": "User name is required"}), 400
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

@app.route("/check_user",methods=['POST'])
def if_user():
    print("if user exists")
    data = request.json
    user_name = data.get("user_name")
    pwd = data.get("pwd")
    if check_user(user_name, pwd):
        return jsonify({"status": "success"})
    else:
        return jsonify({"status": "user already exists"}),400

@app.route("/is_page_flow_server",methods=['GET'])
def is_page_flow_server():
    print("is_page_flow_server")
    return jsonify({"status": "success"}),200

if __name__ == '__main__':
    init_files()
    zip_files_thead(300)
    app.run(host="0.0.0.0", port=5000,debug=True)
    logging.info("server started")