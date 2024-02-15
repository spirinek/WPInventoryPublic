<template>
  <div class="error-page">
    <div class="error-body">
      <div>
        <div class="error-text">404</div>
        <div class="error-sub-text">Страница не найдена</div>
        <div class="error-sub-text">
          Вернемся на главную через
          <span style="color:red">{{leftSeconds}}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
    data(){
        return{
            leftSeconds: 4,
        }
    },
    mounted(){
        setInterval(()=>{
           this.leftSeconds = this.leftSeconds-1;
           if (this.leftSeconds<=0){
                window.location="/";
           }}, 1000);
    }
}
</script>
<style>
.error-page {
  font-family: Arial, Helvetica, sans-serif;
  background-color: black;
  color: white;
  text-align: center;
  padding: 0;
  margin: 0;
  background: rgb(0, 13, 127);
  background: -webkit-linear-gradient(
    left,
    rgba(0, 13, 127, 1) 0%,
    rgba(44, 23, 206, 1) 100%
  );
  background: -moz-linear-gradient(
    left,
    rgba(0, 13, 127, 1) 0%,
    rgba(44, 23, 206, 1) 100%
  );
  background: -o-linear-gradient(
    left,
    rgba(0, 13, 127, 1) 0%,
    rgba(44, 23, 206, 1) 100%
  );
  background: linear-gradient(
    to right,
    rgba(0, 13, 127, 1) 0%,
    rgba(44, 23, 206, 1) 100%
  );
}

.error-body {
  display: flex;
  width: 100vw;
  height: 100vh;
  align-items: center;
  justify-content: center;
}

.error-text {
  font-size: 35px;
  font-weight: bold;
}

.error-sub-text {
  font-size: 16px;
  font-weight: normal;
  margin-top: 10px;
}
</style>