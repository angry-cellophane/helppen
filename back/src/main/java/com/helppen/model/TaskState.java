package com.helppen.model;

public enum TaskState {
    COMPLETED {
        @Override
        public String toString() {
            return TaskStateConstants.COMPLETED;
        }
    },
    NOT_COMPLETED {
        @Override
        public String toString() {
            return TaskStateConstants.NOT_COMPLETED;
        }
    },
    IN_STASH {
        @Override
        public String toString() {
            return TaskStateConstants.IN_STASH;
        }
    };

    public static TaskState of(String state) {
        switch (state) {
            case TaskStateConstants.COMPLETED:
                return COMPLETED;
            case TaskStateConstants.NOT_COMPLETED:
                return NOT_COMPLETED;
            case TaskStateConstants.IN_STASH:
                return IN_STASH;
            default:
                throw new IllegalArgumentException("Unknown state type: " + state);
        }
    }
}
