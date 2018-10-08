ALTER TABLE plum.users
	ADD COLUMN display_name TEXT NOT NULL,
	ADD COLUMN google_claim_nameidentifier TEXT UNIQUE;