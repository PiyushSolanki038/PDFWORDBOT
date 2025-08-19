import telebot
from pdf2docx import Converter
from docx2pdf import convert
import os

TOKEN = "8251029951:AAF7KfQlxOnSJhy_eHwNSIzhzGs1NTqy9ac"
bot = telebot.TeleBot(TOKEN)

@bot.message_handler(commands=['start'])
def send_welcome(message):
    bot.reply_to(message, "Hello, I am alive ✅\nSend me a PDF or Word file to convert.")

@bot.message_handler(content_types=['document'])
def handle_docs(message):
    file_info = bot.get_file(message.document.file_id)
    downloaded = bot.download_file(file_info.file_path)

    file_name = message.document.file_name
    with open(file_name, "wb") as f:
        f.write(downloaded)

    # conversion
    if file_name.endswith(".pdf"):
        output = file_name.replace(".pdf", ".docx")
        cv = Converter(file_name)
        cv.convert(output)
        cv.close()
        bot.send_document(message.chat.id, open(output, "rb"))

    elif file_name.endswith(".docx"):
        output = file_name.replace(".docx", ".pdf")
        convert(file_name, output)
        bot.send_document(message.chat.id, open(output, "rb"))

    else:
        bot.reply_to(message, "❌ Only PDF and DOCX supported.")

bot.polling()
