from PIL import Image

input_path = 'd:\\Kadir\\Projeler\\DefenceDB\\DefenceDB.WebUI\\wwwroot\\images\\owl_teacher.png'
output_path = 'd:\\Kadir\\Projeler\\DefenceDB\\DefenceDB.WebUI\\wwwroot\\images\\owl_teacher_transparent.png'

print("Loading image...")
img = Image.open(input_path).convert("RGBA")
datas = img.getdata()

newData = []
# Anything close to white (R>230, G>230, B>230) becomes transparent
for item in datas:
    if item[0] > 230 and item[1] > 230 and item[2] > 230:
        newData.append((255, 255, 255, 0))
    else:
        newData.append(item)

img.putdata(newData)
img.save(output_path, "PNG")
print("Saved transparent PNG!")
