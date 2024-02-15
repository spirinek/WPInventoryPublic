import axios from 'axios'

const service = axios.create({
    baseURL: window.ServerUrl
})

// respone
service.interceptors.response.use(
    response => {
        return response.data
    }
)

export default service
