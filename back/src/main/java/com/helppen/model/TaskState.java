package com.helppen.model;

public enum TaskState {
    COMPLETED {
        @Override
        public String toString() {
            return TaskStateContants.COMPLETED;
        }
    },
    NOT_COMPLETED {
        @Override
        public String toString() {
            return TaskStateContants.NOT_COMPLETED;
        }
    },
    IN_STASH {
        @Override
        public String toString() {
            return TaskStateContants.IN_STASH;
        }
    };

    public static TaskState of(String state) {
        switch (state) {
            case TaskStateContants.COMPLETED:
                return COMPLETED;
            case TaskStateContants.NOT_COMPLETED:
                return NOT_COMPLETED;
            case TaskStateContants.IN_STASH:
                return IN_STASH;
            default:
                throw new IllegalArgumentException("Unknown state type: " + state);
        }
    }
}
