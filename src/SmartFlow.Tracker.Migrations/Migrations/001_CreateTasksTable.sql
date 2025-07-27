CREATE TABLE IF NOT EXISTS tasks (
                                     id SERIAL PRIMARY KEY,
                                     title TEXT NOT NULL,
                                     status TEXT NOT NULL DEFAULT 'Todo',
                                     created_at TIMESTAMP NOT NULL DEFAULT NOW()
    );
