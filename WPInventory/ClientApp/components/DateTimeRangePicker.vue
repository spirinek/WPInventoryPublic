<template>
  <el-date-picker
    :unlink-panels="true"
    clearable
    v-model="dateFrormAndToArray"
    type="datetimerange"
    :picker-options="pickerOptions"
    @change="changeDate"
    format="dd.MM.yyyy HH:mm"
    value-format="MM/dd/yyyy HH:mm"
    start-placeholder="дата начала"
    end-placeholder="дата окончания"
    :default-time="['00:00:00', '23:59:00']"
    align="center"
  ></el-date-picker>
</template>

<script>
import Vue from "vue";
export default {
  data() {
    return {
      dateFrormAndToArray: ["", ""],
      pickerOptions: {
        shortcuts: [
          {
            text: "За неделю",
            onClick(picker) {
              let startDate = Vue.moment()
                .add(-7, "days").format("MM/DD/YYYY HH:mm");
              let endDate = Vue.moment().hour("23").minutes("59").format("MM/DD/YYYY HH:mm");
              picker.$emit("pick", [startDate, endDate]);
            }
          },
          {
            text: "За месяц",
            onClick(picker) {
              let startDate = Vue.moment().add(-1, "month").format("MM/DD/YYYY HH:mm");
              let endDate = Vue.moment().hour("23").minutes("59").format("MM/DD/YYYY HH:mm");
              picker.$emit("pick", [startDate, endDate]);
            }
          }
        ]
      }
    };
  },
  methods: {
            changeDate (data) {
                if (data === null) {
                    data = [null, null]
                }
                console.log(data)
                this.$emit('changeDateOutSide', data)
            }
        },
};
</script>