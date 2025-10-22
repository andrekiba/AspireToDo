import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Tasks from '../views/Tasks.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/tasks',
    name: 'Tasks',
    component: Tasks
  },
  {
    path: '/newTask',
    name: 'NewTask',
    component: () => import('../views/NewTask.vue')
  },
  {
    path: '/tasks/:id',
    name: 'UpdateTask',
    component: () => import('../views/UpdateTask.vue'),
    props: true
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

router.beforeEach(async (to, from) => {
  
})

export default router
