# script.sh file.mp4 width height framerate
mkdir frames
mkdir bitmaps
ffmpeg -i $1 -r $4 frames/img%04d.png

for i in $(ls ./frames)
do
    convert frames/$i -resize ${2}x${3} bitmaps/${i}.bmp
done
