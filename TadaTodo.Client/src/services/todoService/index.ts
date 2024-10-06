import axios, { type AxiosInstance } from 'axios';
import type { CreateTodoList, TodoList, UpdateTodoList } from '@/models';

export class TodoService {
  private axiosInstance: AxiosInstance;

  constructor() {
    this.axiosInstance = axios;
  }

  public async getTodoLists() {
    const res = await this.axiosInstance.get<TodoList[]>('api/todos');
    return res.data;
  }

  public async getTodoList(id: number) {
    const res = await this.axiosInstance.get<TodoList>(`api/todos/${id}`);
    return res.data;
  }

  public async createTodoList(newList: CreateTodoList) {
    const res = await this.axiosInstance.post<TodoList>('api/todos', newList);
    return res.data;
  }

  public async updateTodoList(todoList: UpdateTodoList) {
    const res = await this.axiosInstance.patch<TodoList>(`api/todos/${todoList.id}`, todoList);
    return res.data;
  }

  public async deleteTodoList(id: number) {
    await this.axiosInstance.delete(`api/todos/${id}`);
    return true;
  }

  public async searchTodoLists(search: string) {
    const res = await this.axiosInstance.get<TodoList[]>('api/todos', { params: { search } });
    return res.data;
  }
}
