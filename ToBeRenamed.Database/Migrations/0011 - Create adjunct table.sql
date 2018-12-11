CREATE TABLE plum.video_tags(
    tag_id INTEGER NOT NULL REFERENCES plum.tags, 
    video_id INTEGER NOT NULL REFERENCES plum.videos )
WITH (OIDS = FALSE);
