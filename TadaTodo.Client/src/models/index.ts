export interface TodoItem {
  id: number;
  value: string;
  isCompleted: boolean;
}
export interface TodoList {
  id: number;
  name: string;
  todoItems: TodoItem[];
}

export interface CreateTodoItem {
  value: string;
  isCompleted: boolean;
}

export interface CreateTodoList {
  name: string;
  todoItems: CreateTodoItem[];
}

export interface UpdateTodoItem {
  id: number;
  value: string;
  isCompleted: boolean;
}

export interface UpdateTodoList {
  id: number;
  name?: string | null;
  newItems?: CreateTodoItem[] | null;
  updatedItems?: UpdateTodoItem[] | null;
  deletedItems?: number[] | null;
}

export interface User {
  id: number;
  username: string;
}

export interface UserLogin {
  username: string;
  password: string;
}
