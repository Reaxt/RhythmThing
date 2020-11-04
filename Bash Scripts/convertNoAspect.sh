# script.sh file.mp4 width height framerate
# THIS ONE WILL FUCK UP THE ASPECT RATIO IF YOU ASK IT TO
mkdir frames
mkdir bitmaps
ffmpeg -i $1 -r $4 frames/img%04d.png

for i in $(ls ./frames)
do
    convert frames/$i -sample ${2}x${3}\! bitmaps/${i}.bmp
done